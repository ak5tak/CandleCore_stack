import type { AnalysisOverviewModel } from '@/features/analysis/types'
import Heading from '@/shared/components/ui/Heading'
import Stack from '@/shared/components/ui/Stack'
import Text from '@/shared/components/ui/Text'
import type { Period, Timeframe } from '@/shared/constants/market'
import AnalysisMarketBehaviour from './AnalysisMarketBehaviour'
import AnalysisMarketSummary from './AnalysisMarketSummary'
import AnalysisPriceChange from './AnalysisPriceChange'
import AnalysisProbability from './AnalysisProbability'
import AnalysisToolbar from './AnalysisToolbar'
import AnalysisRiskAnalysis from './AnalysisRiskAnalysis'

type AnalysisOverviewProps = {
  timeframe: Timeframe
  period: Period
  onTimeframeChange: (timeframe: Timeframe) => void
  onPeriodChange: (period: Period) => void
  model: AnalysisOverviewModel
}

export default function AnalysisOverview({
  timeframe,
  period,
  onTimeframeChange,
  onPeriodChange,
  model,
}: AnalysisOverviewProps) {
  return (
    <Stack gap={8}>
      <Stack
        direction="row"
        gap={4}
        className="flex-wrap items-end justify-between"
      >
        <Stack gap={2}>
          <Heading as={1}>Analysis</Heading>
          <Text muted>
            Statistical insights from historical Bitcoin candle data.
          </Text>
        </Stack>

        <AnalysisToolbar
          timeframe={timeframe}
          period={period}
          onTimeframeChange={onTimeframeChange}
          onPeriodChange={onPeriodChange}
        />
      </Stack>

      <AnalysisMarketSummary metrics={model.marketSummary} />

      <AnalysisPriceChange priceChange={model.priceChange} />

      <AnalysisRiskAnalysis metrics={model.riskAnalysis} />

      <AnalysisMarketBehaviour metrics={model.marketBehaviour} />

      <AnalysisProbability buckets={model.probability} />
    </Stack>
  )
}
