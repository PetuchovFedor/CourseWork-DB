import styles from '../AuthPage/AuthPage.module.css'
import { useState } from 'react';
import { Header } from "../../components/Header/Header";
import { RegistrationDto } from '../../dto/Auth/RegistrationDto';
import { useAuth } from '../../hook/useAuth';
import { AuthResponse } from '../../dto/Auth/AuthDto';
import { useNavigate } from 'react-router-dom';
import { client } from '../../client/client';

// interface IFormInput {
//     name: string;
//     email: string;
//     password: string;
// }

export function RegistrationPage() {

    const [formValues, setFormValues] = useState<RegistrationDto>({
        name: '',
        email: '',
        password: ''
    });
    const authStore = useAuth()
    const navigate = useNavigate()
    const [errors, setErrors] = useState({});
    
    const validate = (values: RegistrationDto) => {
        
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
        }
        return errors;
    }
    
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const{name , value} = e.target;
        setFormValues({...formValues, [name]: value});
    }
    
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) =>{
        e.preventDefault();
        // console.log(formValues)
        setErrors(validate(formValues));
        if (Object.keys(errors).length == 0)
        {
            await client.post<AuthResponse>('auth/registration', formValues)
                .then((response) => {
                    authStore.setTokensToLocalStorage(response.data)
                    authStore.unloadUserFromLocalStorage()
                    navigate('/')
                })
                .catch(error => {
                    console.log(error.response.data)
                    if (error.response.data == 'User with this email already exist') {
                        setErrors({...errors, email: 'Пользователь с такой почтой уже существует'})
                    }
                })
        }
        else {
            console.log('as')
        }
    }
    
    return (
        <>
        {/* <Header /> */}
        <div className={styles.inner}>
            <h2>Регистрация</h2>
                <form onSubmit={handleSubmit}>
                    <label>
                        Email:
                        <input type="email" name = 'email' onChange={handleChange} value={formValues.email} 
                        // pattern='/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i'
                        />
                        <span className={styles.error}>{errors.email}</span>
                    </label>
                    <label>
                        Имя:
                        <input name = 'name' onChange={handleChange} value={formValues.name} 
                        // pattern='/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i'
                        />
                        <span className={styles.error}>{errors.name}</span>
                    </label>
                    <label>
                        Пароль:
                        <input type="password" name = 'password' onChange={handleChange} value={formValues.password} />
                        <span className={styles.error}>{errors.password}</span>
                    </label>
                    <button type="submit">Регистрация</button>
                </form>
        </div>
        </>
       
    )
}