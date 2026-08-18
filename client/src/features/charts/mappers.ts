import type { CandleDto } from '@/api/market-data/types'
import type { CandlePointForChart, ChartSeriesModel } from '@/features/charts/types'
import {
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import { formatDate } from '@/shared/formatting/formatDate'
import type { CandlestickData, UTCTimestamp } from 'lightweight-charts'

function candleTimestamp(candle: CandleDto) {
  return candle.openTime || candle.closeTime
}

export function toChartSeriesModel(
  candles: CandleDto[],
  fallbackSymbol = DEFAULT_SYMBOL,
  fallbackInterval = DEFAULT_TIMEFRAME,
): ChartSeriesModel | null {
  if (candles.length === 0) {
    return null
  }

  const first = candles[0]
  const last = candles[candles.length - 1]
  const points: CandlePointForChart[] = candles.map((candle) => {
    const time = new Date(candleTimestamp(candle)).getTime()

    return {
      time,
      open: candle.open,
      high: candle.high,
      low: candle.low,
      close: candle.close,
      volume: candle.volume,
      isUp: candle.close >= candle.open,
    }
  })

  return {
    symbol: last.symbol || first.symbol || fallbackSymbol,
    interval: last.interval || first.interval || fallbackInterval,
    rangeLabel: `${formatDate(candleTimestamp(first))} – ${formatDate(candleTimestamp(last))}`,
    points,
  }
}

export function toLightweightCandleData(
  points: CandlePointForChart[],
): CandlestickData[] {
  return points.map((point) => ({
    time: Math.floor(point.time / 1000) as UTCTimestamp,
    open: point.open,
    high: point.high,
    low: point.low,
    close: point.close,
  }))
}
