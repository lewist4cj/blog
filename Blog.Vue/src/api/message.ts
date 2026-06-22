import { type BaseResponse, type ListResponse, type ParamsType, http } from './index'

export interface SiteMsgType {
  id: number
  createdAt: string
  type: number
  revUserID: number
  actionUserID: number
  actionUserNickname: string
  actionUserAvatar: string
  title: string
  content: string
  articleID: number
  articleTitle: string
  commentID: number
  isRead: boolean
}

export interface SiteMsgParams extends ParamsType {
  t: 1 | 2 | 3
}

export function siteMsgListApi(params: SiteMsgParams): Promise<BaseResponse<ListResponse<SiteMsgType>>> {
  return http.get('/api/site_msg', { params })
}

export function siteMsgReadApi(data: { id: number; t: 1 | 2 | 3 }): Promise<BaseResponse<string>> {
  return http.post('/api/site_msg', data)
}

export function siteMsgRemoveApi(data: { id: number; t: 1 | 2 | 3 }): Promise<BaseResponse<string>> {
  return http.delete('/api/site_msg', { data })
}

export interface UnReadMsgType {
  commentMsgCount: number
  diggMsgCount: number
  privateMsgCount: number
  systemMsgCount: number
}

export function unReadMsgApi(): Promise<BaseResponse<UnReadMsgType>> {
  return http.get('/api/site_msg/user')
}
