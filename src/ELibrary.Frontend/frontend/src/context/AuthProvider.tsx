import { createContext, useState } from 'react'
import AuthStore  from './AuthStore';

const context = new AuthStore() 
export const AuthContext = createContext<AuthStore>(context)

export const AuthProvider = ({ children }: { children: JSX.Element }) => {
    return (
        <AuthContext.Provider value={context}>
          {children}
        </AuthContext.Provider>
      );
}