import axios from "axios";

type ApiErrorResponse = {
  message?: string;
  title?: string;
  errors?: string[] | Record<string, string[]>;
};

export function getApiErrorMessage(error: unknown, fallback: string) {
  if (!axios.isAxiosError<ApiErrorResponse>(error)) {
    return fallback;
  }

  const data = error.response?.data;
  if (!data) {
    return fallback;
  }

  if (typeof data.message === "string" && data.message.length > 0) {
    return data.message;
  }

  if (Array.isArray(data.errors) && data.errors.length > 0) {
    return data.errors.join(" ");
  }

  if (data.errors && typeof data.errors === "object") {
    const firstError = Object.values(data.errors).flat()[0];
    if (firstError) {
      return firstError;
    }
  }

  return data.title ?? fallback;
}
