import { useContext } from "react"
import { AuthContext } from "../context/AuthProvider"

export function useAuth() {
  const store = useContext(AuthContext)
  // console.log(store)
  return store
}