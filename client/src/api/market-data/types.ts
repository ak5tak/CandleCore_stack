export type CandleDto = {
  id?: string
  symbol: string
  interval: string
  openTime: string
  closeTime: string
  open: number
  high: number
  low: number
  close: number
  volume: number
}

export type CandlesParams = {
  symbol: string
  timeframe: string
  startDate?: string
  endDate?: string
}

export type DatasetBoundsDto = {
  symbol: string
  interval: string
  firstOpenTime: string
  lastOpenTime: string
  candleCount: number
}

export type BoundsParams = {
  symbol: string
  timeframe: string
}
