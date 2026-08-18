import type { ApiClient } from '@/api/httpClient'
import {
  DEFAULT_SYMBOL,
  DEFAULT_TIMEFRAME,
} from '@/shared/constants/defaults'
import { getBounds, getCandles } from './client'
import type { BoundsParams, CandlesParams } from './types'

export function candlesQueryOptions(
  apiClient: ApiClient,
  params: CandlesParams = {
    symbol: DEFAULT_SYMBOL,
    timeframe: DEFAULT_TIMEFRAME,
  },
) {
  return {
    queryKey: ['market-data', 'candles', params] as const,
    queryFn: () => getCandles(apiClient, params),
  }
}

export function boundsQueryOptions(
  apiClient: ApiClient,
  params: BoundsParams = {
    symbol: DEFAULT_SYMBOL,
    timeframe: DEFAULT_TIMEFRAME,
  },
) {
  return {
    queryKey: ['market-data', 'bounds', params] as const,
    queryFn: () => getBounds(apiClient, params),
  }
}
