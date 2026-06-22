import { type BaseResponse, type ListResponse, type OptionsType, type ParamsType, http } from './index'

export interface ArticleListType {
  id: number
  createdAt: string
  updatedAt: string
  title: string
  abstract: string
  categoryId?: number
  categoryTitle?: string
  tagList: string[]
  cover: string
  userId: number
  lookCount: number
  diggCount: number
  commentCount: number
  collectCount: number
  enableComment: boolean
  status: number
  userTop: boolean
  adminTop: boolean
  userNickname: string
  userAvatar: string
}

export interface ArticleListRequest extends ParamsType {
  type?: 1 | 2 | 3
  userId?: number
  collectId?: number
  status?: number
  categoryId?: number
}

export function articleListApi(params: ArticleListRequest): Promise<BaseResponse<ListResponse<ArticleListType>>> {
  return http.get('/api/article', { params })
}

export interface ArticleDetailType {
  id: number
  createdAt: string
  updatedAt: string
  title: string
  abstract: string
  content: string
  categoryId?: number
  categoryTitle?: string
  tagList: string[]
  cover: string
  userId: number
  lookCount: number
  diggCount: number
  commentCount: number
  collectCount: number
  enableComment: boolean
  status: number
  username: string
  nickname: string
  userAvatar: string
  isDigg: boolean
  isCollect: boolean
}

export function articleDetailApi(id: number): Promise<BaseResponse<ArticleDetailType>> {
  return http.get(`/api/article/${id}`)
}

export interface ArticleCreateRequest {
  title: string
  content: string
  abstract?: string
  status?: number
  categoryId?: number
  cover?: string
  tagList?: string
  enableComment?: boolean
}

export function articleCreateApi(data: ArticleCreateRequest): Promise<BaseResponse<{ id: number }>> {
  return http.post('/api/article', data)
}

export interface ArticleEditRequest extends ArticleCreateRequest {
  id: number
}

export function articleEditApi(data: ArticleEditRequest): Promise<BaseResponse<string>> {
  return http.put('/api/article', data)
}

export function articleDeleteApi(id: number): Promise<BaseResponse<string>> {
  return http.delete(`/api/article/${id}`)
}

export function articleExamineApi(data: {
  articleID: number
  status: number
  msg: string
}): Promise<BaseResponse<string>> {
  return http.post('/api/article/examine', data)
}

export function articleDiggApi(id: number): Promise<BaseResponse<{ isDigg: boolean }>> {
  return http.get(`/api/article/digg/${id}`)
}

export function articleCollectApi(data: { articleId: number; collectId: number }): Promise<BaseResponse<string>> {
  return http.post('/api/article/collect', data)
}

export function articleRemoveCollectApi(data: {
  collectId: number
  articleIds: number[]
}): Promise<BaseResponse<string>> {
  return http.delete('/api/article/collect', { data })
}

export function articleHistoryListApi(params: ParamsType): Promise<BaseResponse<ListResponse<ArticleListType>>> {
  return http.get('/api/article/history', { params })
}

export function articleRecordHistoryApi(data: { articleId: number }): Promise<BaseResponse<string>> {
  return http.post('/api/article/history', data)
}

export function articleDeleteHistoryApi(data: { articleId: number }): Promise<BaseResponse<string>> {
  return http.delete('/api/article/history', { data })
}

export function articleTagOptionsApi(): Promise<BaseResponse<OptionsType[]>> {
  return http.get('/api/article/tag/options')
}

export function articleRecommendApi(): Promise<BaseResponse<ArticleListType[]>> {
  return http.get('/api/article/article_recommend')
}

export function articleAuthRecommendApi(): Promise<BaseResponse<ArticleListType[]>> {
  return http.get('/api/article/auth_recommend')
}
