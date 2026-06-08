import * as React from "react";
import { cn } from "@/lib/utils";

type AlertProps = React.ComponentProps<"div"> & {
  variant?: "default" | "success" | "destructive";
};

export function Alert({ className, variant = "default", ...props }: AlertProps) {
  return (
    <div
      role="alert"
      className={cn(
        "relative flex gap-2 rounded-md border px-3 py-2 text-sm",
        variant === "default" && "border-[#B8C1CF] bg-[#F6F5EE] text-[#182748]",
        variant === "success" && "border-[#B8C1CF] bg-[#F6F5EE] text-[#182748]",
        variant === "destructive" && "border-[#DC8C5A] bg-[#FBCBA7]/30 text-[#182748]",
        className,
      )}
      {...props}
    />
  );
}
