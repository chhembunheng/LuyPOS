import * as React from "react";
import { cn } from "@/lib/utils";

export function Input({ className, type, ...props }: React.ComponentProps<"input">) {
  return (
    <input
      type={type}
      className={cn(
        "flex h-11 w-full rounded-md border border-[#B8C1CF] bg-white px-3 py-2 text-base text-[#182748] shadow-xs outline-none transition-colors placeholder:text-[#7C8495] focus:border-[#FF843D] focus:ring-2 focus:ring-[#FBCBA7] disabled:cursor-not-allowed disabled:opacity-50",
        className,
      )}
      {...props}
    />
  );
}
