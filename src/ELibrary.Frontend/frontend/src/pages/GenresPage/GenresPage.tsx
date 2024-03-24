import styles from './GenresPages.module.css'
import { useEffect, useState } from 'react';
import { GenreDto } from "../../dto/Genre/GenreDto";
import { client } from "../../client/client";
import { useAuth } from '../../hook/useAuth';
import { useNavigate } from 'react-router-dom';
import { GenreBlock } from '../../components/GenreBlock/GenreBlock';

export interface IGenreDto {
  Id: number,
  Name: string,
}

interface Genres {
  genres: IGenreDto
}

//https://localhost:7283/genre/get-all
export function GenresPage() {
    const [data, setData] = useState<GenreDto[]>([])
    const [value, setValue] = useState<string>('')
    const authStore = useAuth()
    const navigate = useNavigate()
    const [isAdminHere, setIsAdmin] = useState<boolean>(false)
    const genreList = data.map((genre) =>
      <GenreBlock id={genre.id} name={genre.name} isAdminHere={isAdminHere} />
    ) 
    useEffect(() => {
      const getGenres = async () => {
        await client.get<Array<GenreDto>>('genre/get-genres')
        .then((response) => {
          setData(response.data)
          // console.log(response.data)
          authStore.isAuth && authStore.user.Role == 'admin' ? setIsAdmin(true) : setIsAdmin(false)          
        })
        .catch(error => {
          console.log(error.response.data)
        })
      }
      getGenres()
    }, [])
       
    const onCreate = async (e) => {
      e.preventDefault()
      const dto = {name: value}
      await client.post<GenreDto>('genre/create-genre', dto)
      .then(response => {
        console.log(response.data)
        setData([...data, response.data])
      })
      .catch(error => {
        console.log(error.response.data)
      })
    }

    const onChange = (e)=> {
      e.preventDefault()
      setValue(e.target.value)
      // console.log(e.target.value)
    }
    return (
      <div className={styles.page}>
        {/* <Routes>
          <Route path='/:id' element ={<GenrePage />} />
        </Routes> */}
        <div className={styles.genres_block}>
          {genreList}
        </div>
        {isAdminHere ?
          <div className={styles.create_genre_block}>
            <input className={styles.input} onChange={onChange} value={value} />
            <button className={styles.create_button} onClick={onCreate}>Сохранить</button>
          </div>
        : null}
        {/* {showList ? genreList : null}                     */}
      </div>
    )
    // return (                
    //         <div>
    //           <Routes>
    //           {/* {heroes.map(hero => (<Link to={'heroes/' + hero.id} />)} */}
    //             <Route path="/:id" element= {<GenrePage />}} />
    //           </Routes>
    //         </div>        
    // )
}