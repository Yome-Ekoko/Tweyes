import { ApiResponse } from 'src/types/responseTypes'
import apiService from './ApiService'

export async function apiSignIn (data: any) {
    return apiService.request<ApiResponse<any>, any>({
        url: '/Account/authenticate',
        method: 'post',
        data
    })
}

export async function apiSignOut (data: any) {
    return apiService.request<ApiResponse<any>, any>({
        url: '/Account/logout',
        method: 'post',
        data
    })
}

export async function apiResetPassword (data: any) {
    return apiService.request<ApiResponse<any>, any>({
        url: '/Account/passwordReset',
        method: 'post',
        data
    })
}
