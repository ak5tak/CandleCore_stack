import { QueryClientProvider, type QueryClient } from '@tanstack/react-query'
import { createRouter as createTanStackRouter } from '@tanstack/react-router'
import type { ApiClient } from '@/api/httpClient'
import { apiClient } from '@/api/httpClient'
import { queryClient } from '@/app/queryClient'
import { ErrorComponent } from '@/shared/components/ErrorComponent'
import { NotFoundComponent } from '@/shared/components/NotFoundComponent'
import Spinner from '@/shared/components/ui/Spinner'
import { routeTree } from '../routeTree.gen'

export type RouterContext = {
  queryClient: QueryClient
  apiClient: ApiClient
}

export function createRouter() {
  return createTanStackRouter({
    routeTree,
    scrollRestoration: true,
    defaultPreload: 'intent',
    defaultPreloadStaleTime: 0,
    context: {
      queryClient,
      apiClient,
    } satisfies RouterContext,
    defaultPendingComponent: () => (
      <div className="flex items-center justify-center">
        <Spinner />
      </div>
    ),
    defaultErrorComponent: ErrorComponent,
    defaultNotFoundComponent: NotFoundComponent,
    Wrap: function WrapComponent({ children }) {
      return (
        <QueryClientProvider client={queryClient}>
          {children}
        </QueryClientProvider>
      )
    },
  })
}

export const router = createRouter()

declare module '@tanstack/react-router' {
  interface Register {
    router: ReturnType<typeof createRouter>
  }
}
