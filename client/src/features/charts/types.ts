export type CandlePointForChart = {
  time: number
  open: number
  high: number
  low: number
  close: number
  volume: number
  isUp: boolean
}

export type ChartSeriesModel = {
  symbol: string
  interval: string
  rangeLabel: string
  points: CandlePointForChart[]
}
