import type { MetricForAnalysis } from '@/features/analysis/types'
import Badge from '@/shared/components/ui/Badge'
import Card from '@/shared/components/ui/Card'
import EmptyState from '@/shared/components/ui/EmptyState'
import Grid from '@/shared/components/ui/Grid'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'

type AnalysisMetricSectionProps = {
  title: string
  metrics: MetricForAnalysis[]
  emptyTitle: string
  emptyDescription: string
}

function MetricCard({ metric }: { metric: MetricForAnalysis }) {
  return (
    <Card>
      <Stack gap={2}>
        <Text size="sm" muted>
          {metric.label}
        </Text>
        {metric.tone && metric.tone !== 'neutral' ? (
          <Badge tone={metric.tone} className="w-fit text-sm">
            {metric.value}
          </Badge>
        ) : (
          <Heading as={4}>{metric.value}</Heading>
        )}
        {metric.hint && (
          <Text size="sm" muted>
            {metric.hint}
          </Text>
        )}
      </Stack>
    </Card>
  )
}

export default function AnalysisMetricSection({
  title,
  metrics,
  emptyTitle,
  emptyDescription,
}: AnalysisMetricSectionProps) {
  return (
    <Stack gap={4}>
      <Heading as={3}>{title}</Heading>

      {metrics.length > 0 ? (
        <Grid cols={3}>
          {metrics.map((metric) => (
            <MetricCard key={metric.id} metric={metric} />
          ))}
        </Grid>
      ) : (
        <Card padding="none">
          <EmptyState title={emptyTitle} description={emptyDescription} />
        </Card>
      )}
    </Stack>
  )
}
