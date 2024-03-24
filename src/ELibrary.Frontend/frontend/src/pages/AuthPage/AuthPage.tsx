import { useState } from "react";
import styles from './AuthPage.module.css'
import { Link, useNavigate } from "react-router-dom";
import { Header } from "../../components/Header/Header";
import { client } from "../../client/client";
import { AuthResponse } from "../../dto/Auth/AuthDto";
import { useAuth } from "../../hook/useAuth";


interface IFormInput {
    email: string;
    password: string;
}

export function AuthPage() {
    const [formValues, setFormValues] = useState<IFormInput>({
        email: '',
        password: ''
    });
    const authStore = useAuth()
    const navigate = useNavigate()
    const [errors, setErrors] = useState({});
    
    const validate = (values: IFormInput) => {
        
        const errors = {};
        const regex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;;
    

        if(!values.email){
            errors.email= "Email is required!";
        } else if(!regex.test(values.email)){
            errors.email = "This is not a valid email format!";
        }

        if(!values.password){
            errors.password= "Password is required!";
        } else if(values.password.length < 6){
            errors.password = "Password must be more than 6 characters";
        } else if(values.password.length > 16){
            errors.password = "Password cannot be more than 16 characters";
        }
        return errors;
    }
    
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const{name , value} = e.target;
        setFormValues({...formValues, [name]: value});
    }
    
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) =>{
        e.preventDefault();
        setErrors(validate(formValues));
        if (Object.keys(errors).length == 0) {
            await client.post<AuthResponse>('auth/login', formValues)
            .then(response => {
                authStore.setTokensToLocalStorage(response.data)
                authStore.unloadUserFromLocalStorage()
                console.log(authStore.user, authStore.isAuth)
                navigate('/')
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
    }
    
    return (
        <>
        {/* <Header /> */}
        <div className={styles.inner}>
            <h2>Авторизация</h2>
                <form onSubmit={handleSubmit}>
                    <label>
                        Email:
                        <input type="email" name = 'email' onChange={handleChange} value={formValues.email} 
                        // pattern='/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i'
                        />
                        <span className={styles.error}>{errors.email}</span>
                    </label>
                    <label>
                        Пароль:
                        <input type="password" name = 'password' onChange={handleChange} value={formValues.password} />
                        <span className={styles.error}>{errors.password}</span>
                    </label>
                    <button type="submit">Войти</button>                    
                </form>
            <Link to='/registration'><p>Регистрация</p></Link>
        </div>
        </>        
    )
}