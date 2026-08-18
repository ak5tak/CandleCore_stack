const dateFormatter = new Intl.DateTimeFormat('en-US', {
  dateStyle: 'medium',
  timeZone: 'UTC',
})

const dateTimeFormatter = new Intl.DateTimeFormat('en-US', {
  dateStyle: 'medium',
  timeStyle: 'short',
  timeZone: 'UTC',
})

export function formatDate(value: string | Date) {
  const date = value instanceof Date ? value : new Date(value)
  return dateFormatter.format(date)
}

export function formatDateTime(value: string | Date) {
  const date = value instanceof Date ? value : new Date(value)
  return `${dateTimeFormatter.format(date)} UTC`
}
