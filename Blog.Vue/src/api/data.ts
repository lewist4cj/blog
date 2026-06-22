import { type BaseResponse, http } from './index'

export interface DataSumType {
  articleCount: number
  userCount: number
  commentCount: number
  flowCount: number
  messageCount: number
  newLoginCount: number
  newSignCount: number
}

export function dataSumApi(): Promise<BaseResponse<DataSumType>> {
  return http.get('/api/data/sum')
}

export interface DataGrowthType {
  growthRate: number
  growthNum: number
  countList: number[]
  dateList: number[]
}

export function dataGrowthApi(type: 1 | 2 | 3): Promise<BaseResponse<DataGrowthType>> {
  return http.get('/api/data/growth', { params: { type } })
}

export function dataArticleGrowthApi(): Promise<BaseResponse<DataGrowthType>> {
  return http.get('/api/data/article/year')
}
