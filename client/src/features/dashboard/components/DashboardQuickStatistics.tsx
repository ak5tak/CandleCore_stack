import type { StatisticForOverview } from '@/features/dashboard/types'
import Badge from '@/shared/components/ui/Badge'
import Card from '@/shared/components/ui/Card'
import EmptyState from '@/shared/components/ui/EmptyState'
import Grid from '@/shared/components/ui/Grid'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'

type DashboardQuickStatisticsProps = {
  statistics: StatisticForOverview[]
}

type StatCardProps = {
  statistic: StatisticForOverview
}

function StatCard({ statistic }: StatCardProps) {
  return (
    <Card>
      <Stack gap={2}>
        <Text size="sm" muted>
          {statistic.label}
        </Text>
        {statistic.tone && statistic.tone !== 'neutral' ? (
          <Badge tone={statistic.tone} className="w-fit text-sm">
            {statistic.value}
          </Badge>
        ) : (
          <Heading as={4}>{statistic.value}</Heading>
        )}
        {statistic.hint && (
          <Text size="sm" muted>
            {statistic.hint}
          </Text>
        )}
      </Stack>
    </Card>
  )
}

export default function DashboardQuickStatistics({
  statistics,
}: DashboardQuickStatisticsProps) {
  return (
    <Stack gap={4}>
      <Heading as={3}>Market statistics</Heading>

      {statistics.length > 0 ? (
        <Grid cols={3}>
          {statistics.map((statistic) => (
            <StatCard key={statistic.id} statistic={statistic} />
          ))}
        </Grid>
      ) : (
        <Card padding="none">
          <EmptyState
            title="No statistics yet"
            description="Statistics will appear here once candle data is available."
          />
        </Card>
      )}
    </Stack>
  )
}
