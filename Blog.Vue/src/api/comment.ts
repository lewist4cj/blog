import { type BaseResponse, type ListResponse, type ParamsType, http } from './index'

export interface CommentType {
  id: number
  content: string
  userId: number
  articleId: number
  parentId?: number
  rootParentId?: number
  diggCount: number
  createdAt: string
  nickname: string
  avatar: string
}

export function commentListApi(params: ParamsType): Promise<BaseResponse<ListResponse<CommentType>>> {
  return http.get('/api/comment', { params })
}

export function commentCreateApi(data: {
  content: string
  articleId: number
  parentId?: number
  rootParentId?: number
}): Promise<BaseResponse<string>> {
  return http.post('/api/comment', data)
}

export function commentDeleteApi(id: number): Promise<BaseResponse<string>> {
  return http.delete(`/api/comment/${id}`)
}
