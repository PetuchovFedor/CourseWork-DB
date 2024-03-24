import {jwtDecode} from "jwt-decode"
import { AuthResponse } from '../dto/Auth/AuthDto'
import { makeAutoObservable } from "mobx"

export interface User {
    id: number,
    name: string,
    role: string
}

export default class AuthStore {
    user = {} as User
    isAuth = false
    constructor()
    {
      makeAutoObservable(this)
      this.unloadUserFromLocalStorage()
    }

    decodeToken(token: string): User | null {
        try {
          const result: User = jwtDecode<User>(token) 
          return result
        }
        catch(ex)
        {
          console.log(ex)
          return null
        }
    }

    setTokensToLocalStorage(response: AuthResponse) {
      localStorage.setItem('accessToken', response.accessToken)
      localStorage.setItem('refreshToken', response.refreshToken)
    }

    unloadUserFromLocalStorage() {
      // localStorage.clear()
      // this.isAuth = false
      // this.removeUser()s
      // console.log(this.user, ' ',this.isAuth)
      const token = localStorage.getItem('accessToken')
      if (token)
      {
        const user = this.decodeToken(token)
        if (user)
        {
          this.setUser(user)
          console.log(user)
        }
      }
    }

    removeUser() {
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
      this.isAuth = false
      this.user = {} as User
    }
    
    private setUser(user: User) {
      this.user = user
      this.isAuth = true
    }
}