import type { PriceChangeForAnalysis } from '@/features/analysis/types'
import Badge from '@/shared/components/ui/Badge'
import Card from '@/shared/components/ui/Card'
import EmptyState from '@/shared/components/ui/EmptyState'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'

type AnalysisPriceChangeProps = {
  priceChange: PriceChangeForAnalysis | null
}

export default function AnalysisPriceChange({
  priceChange,
}: AnalysisPriceChangeProps) {
  return (
    <Stack gap={4}>
      <Heading as={3}>Price Change</Heading>

      {priceChange ? (
        <Card>
          <Stack gap={3}>
            <Text size="sm" muted>
              Price Change
            </Text>
            <Badge tone={priceChange.tone} className="w-fit text-base">
              {priceChange.formattedChangePercent}
            </Badge>
            <Stack direction="row" gap={6} className="flex-wrap">
              <Stack gap={1}>
                <Text size="sm" muted>
                  Start Price
                </Text>
                <Text>{priceChange.formattedOldestClose}</Text>
              </Stack>
              <Stack gap={1}>
                <Text size="sm" muted>
                  End Price
                </Text>
                <Text>{priceChange.formattedLatestClose}</Text>
              </Stack>
            </Stack>
          </Stack>
        </Card>
      ) : (
        <Card padding="none">
          <EmptyState
            title="No price change"
            description="Price change will appear once enough candles are available for this range."
          />
        </Card>
      )}
    </Stack>
  )
}
