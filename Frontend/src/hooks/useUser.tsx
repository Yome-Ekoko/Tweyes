import { Alert, AlertTitle, Avatar, Box, Icon, IconButton, Typography } from '@mui/material'
import useRequest from './useRequest'
import { UserQueryParams } from 'src/types/queryTypes'
import { GetUserResponse, PagedApiResponse, PendingUserResponse } from 'src/types/responseTypes'

export function useFetchUser(params: UserQueryParams) {
  const { data, error, isValidating, isLoading, mutate } = useRequest<PagedApiResponse<GetUserResponse[]>>(
    {
      url: '/api/User/getUsers',

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

export function useFetchPendingUser(params: UserQueryParams) {
  const { data, error, isValidating, isLoading, mutate } = useRequest<PagedApiResponse<PendingUserResponse[]>>(
    {
      url: '/api/User/pendingUserRequest',

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
