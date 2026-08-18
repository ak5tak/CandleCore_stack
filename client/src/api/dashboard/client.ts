import type { ApiClient } from '@/api/httpClient'
import { buildQueryPath } from '@/api/buildQueryPath'
import type { DashboardOverviewDto, DashboardOverviewParams } from './types'

export function getDashboardOverview(
  client: ApiClient,
  params: DashboardOverviewParams,
) {
  return client.get<DashboardOverviewDto | null>(
    buildQueryPath('/api/dashboard/overview', params),
  )
}
