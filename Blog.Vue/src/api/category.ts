import { type BaseResponse, http } from './index'

export interface CategoryType {
  id: number
  title: string
  userId: number
  createdAt: string
}

export function categoryListApi(params: {
  page?: number
  limit?: number
  key?: string
  userId?: number
  type?: number
}): Promise<BaseResponse<{ list: CategoryType[]; count: number }>> {
  return http.get('/api/category', { params })
}

export function categoryOptionsApi(): Promise<BaseResponse<{ value: number; label: string }[]>> {
  return http.get('/api/category/options')
}

export function categoryCreateApi(data: { title: string }): Promise<BaseResponse<string>> {
  return http.post('/api/category', data)
}

export function categoryDeleteApi(data: { idList: number[] }): Promise<BaseResponse<string>> {
  return http.delete('/api/category', { data })
}
