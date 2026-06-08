"use client";

import Image from "next/image";
import Link from "next/link";
import { useRouter } from "next/navigation";
import axios from "axios";
import {
  BadgeCheck,
  ChevronRight,
  KeyRound,
  LoaderCircle,
  LogIn,
  ShieldCheck,
  UserRound,
} from "lucide-react";
import { FormEvent, useState } from "react";
import { Alert } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { getApiErrorMessage } from "@/lib/api-error";

type LoginResponse = {
  message?: string;
  success?: boolean;
  username?: string;
  token?: string;
  refreshToken?: string;
  roles?: string[];
  tokenType?: string;
  expiresIn?: number;
};

export default function LoginPage() {
  const router = useRouter();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [message, setMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setIsSubmitting(true);
    setMessage("");
    setIsSuccess(false);

    try {
      const { data } = await axios.post<LoginResponse>("/api/auth/login", {
        username,
        password,
      });

      if (data.success === false) {
        setMessage(data.message ?? "Login failed.");
        return;
      }

      localStorage.setItem(
        "luypos.auth",
        JSON.stringify({
          username: data.username,
          token: data.token,
          refreshToken: data.refreshToken,
          roles: data.roles ?? [],
          tokenType: data.tokenType ?? "Bearer",
          expiresIn: data.expiresIn,
        }),
      );
      setIsSuccess(true);
      setMessage(data.message ?? "Login successful.");
      router.push("/dashboard");
    } catch (error) {
      setMessage(getApiErrorMessage(error, "Unable to connect to the server."));
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <main className="min-h-dvh bg-[#F6F5EE] text-[#182748] lg:h-dvh lg:overflow-hidden">
      <section className="grid min-h-dvh w-full grid-cols-1 lg:h-dvh lg:grid-cols-[minmax(34rem,44rem)_minmax(0,1fr)]">
        <aside className="relative flex min-h-dvh flex-col overflow-hidden bg-[#182748] px-7 py-7 text-white sm:px-14 sm:py-12 lg:h-dvh lg:min-h-0">
          <div
            className="absolute right-0 top-0 h-80 w-[31rem] bg-[#FF843D]"
            style={{ clipPath: "polygon(34% 0, 100% 0, 100% 100%, 0 18%)" }}
          />
          <div
            className="absolute bottom-0 left-0 h-44 w-[30rem] bg-[#3A4562]/70"
            style={{ clipPath: "polygon(0 42%, 100% 0, 78% 100%, 0 100%)" }}
          />

          <div className="relative z-10">
            <Image
              src="/luypos-brand.png"
              alt="LuyPOS"
              width={420}
              height={420}
              className="h-44 w-44 object-contain drop-shadow-2xl sm:h-52 sm:w-52"
              priority
            />
          </div>

          <div className="relative z-10 mt-auto max-w-xl pb-14">
            <p className="flex items-center gap-2 text-sm font-semibold uppercase tracking-[0.18em] text-[#FBCBA7]">
              <ShieldCheck className="size-4" aria-hidden />
              Staff Access
            </p>
            <h1 className="mt-5 text-5xl font-semibold leading-tight tracking-normal sm:text-6xl">
              Welcome back
            </h1>
            <p className="mt-5 max-w-md text-base leading-7 text-[#B8C1CF]">
              Sign in to continue managing your LuyPOS workspace.
            </p>
          </div>
        </aside>

        <div className="relative flex min-h-dvh items-center justify-center overflow-hidden px-6 py-8 sm:px-12 lg:h-dvh lg:min-h-0">
          <div
            className="absolute right-0 top-0 h-80 w-[28rem] bg-[#FBCBA7]/75"
            style={{ clipPath: "polygon(36% 0, 100% 0, 100% 100%, 0 30%)" }}
          />
          <div
            className="absolute bottom-0 left-0 h-64 w-[32rem] bg-white/80"
            style={{ clipPath: "polygon(0 0, 78% 24%, 100% 100%, 0 100%)" }}
          />

          <Card className="relative z-10 w-full max-w-lg border-white/80 bg-white/95 p-7 shadow-2xl shadow-[#182748]/12 backdrop-blur sm:p-10">
            <CardHeader className="mb-7 space-y-0">
              <div className="mb-5 flex items-center justify-between gap-4">
                <div className="rounded-md bg-[#182748] px-3 py-2 text-xs font-semibold uppercase tracking-[0.14em] text-white">
                  LuyPOS ID
                </div>
                <Button asChild variant="outline" size="sm">
                  <Link href="/register">
                    Register
                    <ChevronRight aria-hidden />
                  </Link>
                </Button>
              </div>
              <CardTitle className="text-5xl">Sign in</CardTitle>
              <CardDescription>Use your LuyPOS staff account.</CardDescription>
            </CardHeader>

            <CardContent>
              <form className="space-y-5" onSubmit={handleSubmit}>
                <div className="space-y-2">
                  <Label htmlFor="username">Username or email</Label>
                  <div className="relative">
                    <UserRound className="pointer-events-none absolute left-3 top-1/2 size-4 -translate-y-1/2 text-[#7C8495]" aria-hidden />
                    <Input
                      id="username"
                      value={username}
                      onChange={(event) => setUsername(event.target.value)}
                      className="pl-9"
                      autoComplete="username"
                      required
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="password">Password</Label>
                  <div className="relative">
                    <KeyRound className="pointer-events-none absolute left-3 top-1/2 size-4 -translate-y-1/2 text-[#7C8495]" aria-hidden />
                    <Input
                      id="password"
                      value={password}
                      onChange={(event) => setPassword(event.target.value)}
                      className="pl-9"
                      type="password"
                      autoComplete="current-password"
                      required
                    />
                  </div>
                </div>

                {message ? (
                  <Alert variant={isSuccess ? "success" : "destructive"}>
                    {isSuccess ? (
                      <BadgeCheck className="mt-0.5 size-4 text-[#FF843D]" aria-hidden />
                    ) : (
                      <ShieldCheck className="mt-0.5 size-4 text-[#DC8C5A]" aria-hidden />
                    )}
                    <span>{message}</span>
                  </Alert>
                ) : null}

                <Button type="submit" disabled={isSubmitting} className="h-12 w-full">
                  {isSubmitting ? (
                    <LoaderCircle className="animate-spin" aria-hidden />
                  ) : (
                    <LogIn aria-hidden />
                  )}
                  {isSubmitting ? "Signing in..." : "Sign in"}
                </Button>
              </form>
            </CardContent>
          </Card>
        </div>
      </section>
    </main>
  );
}
