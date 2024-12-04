import apiService from './ApiService'
import { DownloadLogRequest } from 'src/types/requestTypes'
import toast from 'react-hot-toast'
import { ApiResponse } from 'src/types/responseTypes';

export async function apiDownloadWithdrawLogs (params: DownloadLogRequest) {
    return apiService.request<BlobPart, DownloadLogRequest>({
        url: '/api/WithdrawLogs/download',
        method: 'get',
        responseType: "blob",
        params
    })
    .catch(async err => {
      const errorResponse = JSON.parse(await err.response.data.text()) as ApiResponse<string>;
      toast.error(errorResponse?.message ?? "An error occured.")
    })
}
