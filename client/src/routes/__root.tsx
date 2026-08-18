import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import Navbar from '@/shared/components/Navbar'
import { ThemeProvider } from '@/shared/components/ThemeProvider'
import type { RouterContext } from '@/app/router'

function RootLayout() {
  return (
    <ThemeProvider>
      <div className="mx-auto flex min-h-screen w-[80%] flex-col pb-8">
        <Navbar />
        <div className="w-full">
          <Outlet />
        </div>
      </div>
      <TanStackRouterDevtools />
      <ReactQueryDevtools />
    </ThemeProvider>
  )
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: RootLayout,
})
