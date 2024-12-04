import useRequest from './useRequest'
import { FundLogQueryParams } from 'src/types/queryTypes'
import { GetFundLogResponse, PagedApiResponse } from 'src/types/responseTypes'

export function useFetchFundLog(params: FundLogQueryParams) {
  const { data, error, isValidating, isLoading, mutate } = useRequest<PagedApiResponse<GetFundLogResponse[]>>(
    {
      url: '/api/FundLogs/logs',

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
