import type { ApiClient } from '@/api/httpClient'
import { buildQueryPath } from '@/api/buildQueryPath'
import type { AnalysisOverviewDto, OverviewParams } from './types'

function buildAnalysisPath(
  endpoint: string,
  params: Record<string, string | number | undefined>,
) {
  return buildQueryPath(`/api/analysis/${endpoint}`, params)
}

export function getAnalysisOverview(client: ApiClient, params: OverviewParams) {
  return client.get<AnalysisOverviewDto | null>(
    buildAnalysisPath('overview', params),
  )
}
