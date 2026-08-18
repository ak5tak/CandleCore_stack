import { useEffect, useRef } from 'react'
import {
  CandlestickSeries,
  ColorType,
  createChart,
  type IChartApi,
  type ISeriesApi,
} from 'lightweight-charts'
import { toLightweightCandleData } from '@/features/charts/mappers'
import type { CandlePointForChart } from '@/features/charts/types'
import { useTheme } from '@/shared/components/ThemeProvider'

const UP_COLOR = '#16a34a'
const DOWN_COLOR = '#dc2626'

type ChartCandlesProps = {
  points: CandlePointForChart[]
}

function resolveIsDark(theme: 'dark' | 'light' | 'system') {
  if (theme === 'dark') {
    return true
  }

  if (theme === 'light') {
    return false
  }

  return window.matchMedia('(prefers-color-scheme: dark)').matches
}

function getChartColors(isDark: boolean) {
  return {
    background: isDark ? '#16171d' : '#ffffff',
    text: isDark ? '#9ca3af' : '#6b6375',
    grid: isDark ? '#2e303a' : '#e5e4e7',
    border: isDark ? '#2e303a' : '#e5e4e7',
  }
}

export default function ChartCandles({ points }: ChartCandlesProps) {
  const containerRef = useRef<HTMLDivElement>(null)
  const chartRef = useRef<IChartApi | null>(null)
  const seriesRef = useRef<ISeriesApi<'Candlestick'> | null>(null)
  const { theme } = useTheme()

  useEffect(() => {
    const container = containerRef.current
    if (!container) {
      return
    }

    const colors = getChartColors(resolveIsDark(theme))
    const chart = createChart(container, {
      autoSize: true,
      layout: {
        background: { type: ColorType.Solid, color: colors.background },
        textColor: colors.text,
      },
      grid: {
        vertLines: { color: colors.grid },
        horzLines: { color: colors.grid },
      },
      rightPriceScale: {
        borderColor: colors.border,
      },
      timeScale: {
        borderColor: colors.border,
        timeVisible: true,
        secondsVisible: false,
      },
    })

    const series = chart.addSeries(CandlestickSeries, {
      upColor: UP_COLOR,
      downColor: DOWN_COLOR,
      borderVisible: false,
      wickUpColor: UP_COLOR,
      wickDownColor: DOWN_COLOR,
    })

    chartRef.current = chart
    seriesRef.current = series

    return () => {
      chart.remove()
      chartRef.current = null
      seriesRef.current = null
    }
  }, [theme])

  useEffect(() => {
    const series = seriesRef.current
    const chart = chartRef.current
    if (!series || !chart) {
      return
    }

    series.setData(toLightweightCandleData(points))
    chart.timeScale().fitContent()
  }, [points, theme])

  return <div ref={containerRef} className="h-[28rem] w-full min-w-0" />
}
