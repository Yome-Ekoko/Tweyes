import useRequest from './useRequest'
import { AccountLogQueryParams } from 'src/types/queryTypes'
import { GetAccountLogResponse, PagedApiResponse } from 'src/types/responseTypes'

export function useFetchAccountLog(params: AccountLogQueryParams) {
  const { data, error, isValidating, isLoading, mutate } = useRequest<PagedApiResponse<GetAccountLogResponse[]>>(
    {
      url: '/api/AccountLogs/logs',

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
