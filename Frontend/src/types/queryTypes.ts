import { DateType } from "./generalTypes"

export interface QueryParams {
  pageNumber?: number
  pageSize?: number
}

export interface UserQueryParams extends QueryParams {
  query: string
  role: string
  status: number | undefined
}

export interface AccountLogQueryParams extends QueryParams {
  startDate: DateType
  endDate: DateType
}

export interface FundLogQueryParams extends QueryParams {
  startDate: DateType
  endDate: DateType
}

export interface WithdrawLogQueryParams extends QueryParams {
  startDate: DateType
  endDate: DateType
}


