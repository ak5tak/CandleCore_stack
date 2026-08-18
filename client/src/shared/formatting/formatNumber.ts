const numberFormatter = new Intl.NumberFormat('en-US')

const volumeFormatter = new Intl.NumberFormat('en-US', {
  maximumFractionDigits: 2,
})

export function formatNumber(value: number) {
  return numberFormatter.format(value)
}

export function formatVolume(value: number) {
  return volumeFormatter.format(value)
}
