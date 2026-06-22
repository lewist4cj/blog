import { type BaseResponse, type ListResponse, type ParamsType, http } from './index'

export interface CollectType {
  id: number
  title: string
  abstract: string
  cover: string
  articleCount: number
  userId: number
  createdAt: string
}

export function collectListApi(userId: number): Promise<BaseResponse<CollectType[]>> {
  return http.get('/api/collect', { params: { userId } })
}

export function collectCreateApi(data: { title: string; abstract?: string }): Promise<BaseResponse<string>> {
  return http.post('/api/collect', data)
}

export function collectDeleteApi(data: { idList: number[] }): Promise<BaseResponse<string>> {
  return http.delete('/api/collect', { data })
}
