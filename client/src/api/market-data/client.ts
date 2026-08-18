import type { ApiClient } from '@/api/httpClient'
import { buildQueryPath } from '@/api/buildQueryPath'
import type {
  BoundsParams,
  CandleDto,
  CandlesParams,
  DatasetBoundsDto,
} from './types'

function buildMarketDataPath(
  endpoint: string,
  params: Record<string, string | number | undefined>,
) {
  return buildQueryPath(`/api/market-data/${endpoint}`, params)
}

export function getCandles(client: ApiClient, params: CandlesParams) {
  return client.get<CandleDto[]>(buildMarketDataPath('candles', params))
}

export function getBounds(client: ApiClient, params: BoundsParams) {
  return client.get<DatasetBoundsDto | null>(
    buildMarketDataPath('bounds', params),
  )
}
