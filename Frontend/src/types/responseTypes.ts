export interface ApiResponse<T> {
  succeeded: boolean
  code: number
  message: string
  data: T,
  error: string[]
}

export interface PagedApiResponse<T> {
  succeeded: boolean
  code: number
  message: string
  data: T,
  error: string[],
  pageMeta: PageMeta
}

export interface PageMeta {
  pageNumber: number
  pageSize: number
  totalPages: number
  totalRecords: number
}

export interface GetUserResponse {
  id: string
  userName: string
  name: string
  status: number
  statusMeaning: string
  email: string
  lockoutEnd: Date
  lockoutEnabled: boolean
  accessFailedCount: number
  createdAt: Date
  roles: string[]
}

export interface PendingUserResponse {
  id: string
  userName: string
  firstName: string
  lastName: string
  email: string
  role: string
  requestType: string
  participantId: string
  initiator: string
  dateInitiated: Date
}

export interface GetAccountLogResponse {
  id: number
  accountNumber: string
  createdDate: Date
  userId: string
}

export interface GetFundLogResponse {
  id: number
  amount: string
  attempts: number
  createdDate: Date
  debitAccountNumber: string
  lastModifiedDate: Date
  responseCode: string
  responseMessage: string
  rvslResponseCode: number
  rvslResponseMessage: string
  status: string
  transactionReference: string
  userId: string
}

export interface GetWithdrawLogResponse {
  id: number
  amount: string
  createdDate: Date
  creditAccountNumber: string
  responseCode: number
  responseMessage: string
  status: string
  transactionReference: string
  userId: string
}
