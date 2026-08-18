const percentFormatter = new Intl.NumberFormat('en-US', {
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
  signDisplay: 'exceptZero',
})

const unsignedPercentFormatter = new Intl.NumberFormat('en-US', {
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
})

export function formatPercentage(value: number) {
  return `${percentFormatter.format(value)}%`
}

export function formatUnsignedPercentage(value: number) {
  return `${unsignedPercentFormatter.format(value)}%`
}
