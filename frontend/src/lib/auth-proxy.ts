const apiBaseUrl =
  process.env.API_BASE_URL ??
  process.env.NEXT_PUBLIC_API_BASE_URL ??
  "http://localhost:5178";

export async function proxyAuthRequest(request: Request, path: string) {
  const body = await request.text();

  const apiResponse = await fetch(`${apiBaseUrl}${path}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body,
  });

  const responseBody = await apiResponse.text();

  return new Response(responseBody, {
    status: apiResponse.status,
    headers: {
      "Content-Type": apiResponse.headers.get("Content-Type") ?? "application/json",
    },
  });
}
