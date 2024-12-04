import useRequest from './useRequest'
import { WithdrawLogQueryParams } from 'src/types/queryTypes'
import { GetWithdrawLogResponse, PagedApiResponse } from 'src/types/responseTypes'

export function useFetchWithdrawLog(params: WithdrawLogQueryParams) {
  const { data, error, isValidating, isLoading, mutate } = useRequest<PagedApiResponse<GetWithdrawLogResponse[]>>(
    {
      url: '/api/WithdrawLogs/logs',

      method: 'GET',
      params
    },
    {
      dedupingInterval: 50000
    }
  )

  return {
    data: data?.data ?? [],
    pageMeta: data?.pageMeta,
    error,
    isValidating,
    isLoading,
    mutate
  }
}
