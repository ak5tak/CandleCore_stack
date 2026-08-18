export function buildQueryPath(
  basePath: string,
  params: Record<string, string | number | undefined>,
) {
  const searchParams = new URLSearchParams()

  for (const [key, value] of Object.entries(params)) {
    if (value === undefined) {
      continue
    }

    searchParams.set(key, String(value))
  }

  const queryString = searchParams.toString()
  return `${basePath}${queryString ? `?${queryString}` : ''}`
}
