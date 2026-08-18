const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? ''

export class ApiError extends Error {
  status: number

  constructor(status: number, message: string) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

export function isApiError(error: unknown): error is ApiError {
  return error instanceof ApiError
}

export type ApiGetOptions = {
  headers?: HeadersInit
}

export type ApiClient = {
  get: <T>(path: string, options?: ApiGetOptions) => Promise<T>
}

async function getRequest<T>(
  path: string,
  options?: ApiGetOptions,
): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: 'GET',
    headers: {
      Accept: 'application/json',
      ...options?.headers,
    },
  })

  if (!response.ok) {
    throw new ApiError(
      response.status,
      `API request failed: ${response.status} ${response.statusText}`,
    )
  }

  if (response.status === 204) {
    return null as T
  }

  const text = await response.text()
  if (!text) {
    return null as T
  }

  return JSON.parse(text) as T
}

export const apiClient: ApiClient = {
  get: <T,>(path: string, options?: ApiGetOptions) =>
    getRequest<T>(path, options),
}
