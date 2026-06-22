import axios from 'axios'
import { ElMessage } from 'element-plus'
import { useUserStore } from '@/stores/user'

export interface BaseResponse<T> {
  code: number
  message: string
  data: T
}

export interface ListResponse<T> {
  list: T[]
  count?: number
  totalCount?: number
}

export interface ParamsType {
  key?: string
  limit?: number
  page?: number
  sort?: string
  [key: string]: unknown
}

export const http = axios.create({
  timeout: 10000,
  baseURL: '',
})

http.interceptors.request.use((config) => {
  const userStore = useUserStore()
  if (userStore.token) {
    config.headers.set('Authorization', `Bearer ${userStore.token}`)
  }
  return config
})

http.interceptors.response.use(
  (res) => {
    if (res.status === 200) {
      return res.data
    }
    return res
  },
  (err) => {
    ElMessage.error(err.message || '请求失败')
    return Promise.reject(err)
  },
)

export function defaultDeleteApi(url: string, data: { idList: number[] }): Promise<BaseResponse<string>> {
  return http.delete(url, { data })
}

export function defaultPostApi(url: string, data: unknown): Promise<BaseResponse<string>> {
  return http.post(url, data)
}

export function defaultPutApi(url: string, data: unknown): Promise<BaseResponse<string>> {
  return http.put(url, data)
}

export interface OptionsType {
  label: string
  value: number | string
}
