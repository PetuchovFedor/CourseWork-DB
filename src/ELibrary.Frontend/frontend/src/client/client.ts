import axios from "axios"
import { config } from "process"
import { AuthResponse } from "../dto/Auth/AuthDto"
import { useAuth } from "../hook/useAuth"

const BASE_URL = 'https://localhost:7283/api/'

export const client = axios.create({
    baseURL: BASE_URL
})

client.interceptors.request.use((config) => {
    if (config.authorization !== false) {
        console.log('adsa')
        const token = localStorage.getItem('accessToken')
        if (token) {
            config.headers.Authorization = "Bearer " + token
        }
    }
    return config
})

client.interceptors.response.use(
    (response) => {
    return response
},
    async (error) => {
        const originalRequest = error.config;
        // console.log(error.response)
        const refreshToken = localStorage.getItem('refreshToken')
        const accessToken = localStorage.getItem('accessToken')
        if (
            refreshToken &&
            error.response?.status === 401 &&
            originalRequest?.url !== REFRESH_ACCESS_TOKEN_URL &&
            originalRequest?._retry !== true
          )
          {
            originalRequest._retry = true;
            try {
                const dto: AuthResponse = {accessToken: accessToken, refreshToken: refreshToken}
                const response = await client.post('token/refresh', dto)
                localStorage.setItem('accessToken', response.data.accessToken)
                return client.request(originalRequest)
            }
            catch(e) {
                const authStore = useAuth()
                const response = await client.get('auth/logout/' + authStore.user.Id)
                authStore.removeUser()
            }
          }
        //   originalRequest._retry = true;
        return Promise.reject(error);
        
    })