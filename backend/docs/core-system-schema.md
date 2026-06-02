# Core System Schema Design

This design is based on the existing MariaDB `core_system` dump and is mapped for the LuyPOS ASP.NET Core backend using Entity Framework Core with PostgreSQL.

## Included Tables

The application model includes these active business tables:

- `users`, `user_profiles`, `user_translations`
- `languages`
- `roles`, `role_translations`
- `permissions`, `permission_translations`
- `menus`, `menu_translations`
- `user_roles`, `user_permissions`, `role_permissions`, `menu_permissions`
- `user_sessions`, `refresh_tokens`, `user_otp`, `user_forgot_password`
- `login_histories`, `user_activities`
- `audit_logs`, `audit_log_details`

The dump-only tables `flyway_schema_history` and `user_activities_backup_20260421` are intentionally not mapped because they are migration/history artifacts, not application domain tables.

## Relationship Summary

- A user has one profile and many translations, roles, direct permissions, sessions, refresh tokens, OTPs, forgot-password tokens, login histories, audit logs, and activity records.
- Languages are shared by user, role, permission, and menu translation tables.
- Roles and permissions are many-to-many through `role_permissions`.
- Users can receive roles through `user_roles` and direct permissions through `user_permissions`.
- Menus are self-referencing through `parent_id` and can be linked to permissions through `menu_permissions`.
- Audit logs can optionally reference a user and have many detail rows.

## PostgreSQL Adjustments

The original MariaDB schema uses composite unique keys such as:

```sql
UNIQUE KEY (user_id, role_id, deleted_at)
```

That pattern does not reliably enforce one active row when `deleted_at` is `NULL`. In PostgreSQL, the EF mapping uses partial unique indexes instead:

```sql
CREATE UNIQUE INDEX ... ON user_roles (user_id, role_id)
WHERE deleted_at IS NULL;
```

This keeps soft deletes and correctly prevents duplicate active assignments.

## Notes

- `DateTime` fields are mapped with EF defaults and app-side timestamp stamping on save.
- Delete behavior follows the source schema: user profile, OTP, forgot-password, and activity rows cascade with user deletion; most access-control and audit relationships restrict deletion.
- The existing MariaDB seed data was not imported. It should be recreated as PostgreSQL seed data or EF migrations when `dotnet-ef` is available.
