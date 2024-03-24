// import OutlinedInput from '@mui/material/OutlinedInput';
// import InputLabel from '@mui/material/InputLabel';
// import MenuItem from '@mui/material/MenuItem';
// import FormControl from '@mui/material/FormControl';
// import ListItemText from '@mui/material/ListItemText';
// import Select, { SelectChangeEvent } from '@mui/material/Select';
// import Checkbox from '@mui/material/Checkbox';
import { useEffect, useState } from 'react';
import { GenreDto } from '../../dto/Genre/GenreDto';
import { useAuth } from '../../hook/useAuth';
import { useNavigate } from 'react-router-dom';
import { client } from '../../client/client';
import Multiselect from 'multiselect-react-dropdown';

// const ITEM_HEIGHT = 25;
// const ITEM_PADDING_TOP = 8;
// const MenuProps = {
//   PaperProps: {
//     style: {
//       maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
//       width: 150,
//     },
//   },
// };

interface IGenresMenuProps {
    id: number,
    setGenres: React.Dispatch<React.SetStateAction<number[]>>
    selectedGenres: number[]
}

// let genreMap = new Map<number, string>()
// let reverseGenreMap = new Map<string, number>()

export function GenresMenu(props: IGenresMenuProps) {
    const [genres, setGenres] = useState<Array<GenreDto>>([])
    // const [genreMap, setMap] = useState<Map<number, string>>(new Map<number, string>())
    // const [genreMap, setMap] = useState<Map<string, number>>() 
    const [selectedGenres, selectGenres] = useState<Array<GenreDto>>([])   
    const authStore = useAuth()
    const navigate = useNavigate()        
    useEffect(() => {
        const getGenres = async () => {
            await client.get<Array<GenreDto>>('genre/get-genres')
            .then(response => {
                // console.log(response.data)
                setGenres(response.data)
                let temp = []
                response.data.forEach(genre => {
                  if (props.selectedGenres.indexOf(genre.id) != -1) {
                    temp.push({id: genre.id, name: genre.name})
                    // selectGenres([...selectedGenres, {id: genre.id, name: genre.name}])
                    // console.log(temp)
                  }
                  selectGenres(temp)
                })
                // let temp: Map<number, string> = new Map<number, string>()
                // response.data.forEach(genre => {
                //   temp.set(genre.id, genre.name)
                // })
                // if (genreMap.size == 0) {
                  // genres?.forEach((genre)=> {genreMap.set(genre.id, genre.name)})
                  // genreMap.forEach((value: string, key: number) => {
                  //   reverseGenreMap.set(value, key)
                  // })
                  // console.log(genreMap)
                // }               
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }       
        getGenres()
        // let temp = new Map<string, number>()
        // for (let item of genres) {
        //   temp.set(item.name, item.id)
        // }
        // genres?.
        // genres?.forEach((genre) => {})
        // console.log(temp)
        // setMap(temp)
    }, [props.selectedGenres])

    // useEffect(() => {
    //   // console.log(selectedGenres)
    //   props.setGenres(selectedGenres.map(genre => {
    //     return genre.id
    //   }))

    // }, [selectedGenres])
    const onSelect = (selectedList, selectedItem) => {
      selectGenres(selectedList)
      props.setGenres(selectedList.map(item => {
        return item.id
      }))
      // console.log(selectedList)
      // console.log(selectedGenres)
      // console.log(selectedItem)
      // const temp = selectedList.map(item => {
      //   return item.id
      // })
      // props.setGenres(temp)
      // console.log(selectedList.map(item => {
      //   return item.id
      // }))
    }
    const onRemove = (selectedList, removedItem) => {
      selectGenres(selectedList)
      props.setGenres(selectedList.map(item => {
        return item.id
      }))
      // const temp = selectedList.map(item => {
      //   return item.id
      // })
      // props.setGenres(temp)

    }
    return (
        <div>
          <Multiselect
          id={props.id.toString()}
          
          displayValue={'name'}
          options={genres}
          selectedValues={selectedGenres}
          onSelect={onSelect}
          onRemove={onRemove}
          // disablePreSelectedValues
          // preSelectedValues={props.selectedGenres.map(props.selectedGenres.map((genre) => {
          //   return {id: genre, name: genreMap.get(genre)}
          // }))}
          />
        </div>
    )
}
{/* <FormControl sx={{ m: 1, width: 350 }}>
<InputLabel id="demo-multiple-checkbox-label">Жанры</InputLabel>
<Select
  labelId="demo-multiple-checkbox-label"
  id="demo-multiple-checkbox"
  multiple
  value={selectedGenres.map(item => {
    return genreMap.get(item)
  })}
  onChange={handleChange}
  input={<OutlinedInput label="Жанры" />}
  renderValue={(selected) => selected.join(', ')}
  MenuProps={MenuProps}
>
  {genres && genres.map((genre) => (
    <MenuItem key={genre.id} value={genre.name}>
      <Checkbox checked={selectedGenres.indexOf(genre.id) > -1} />
      <ListItemText primary={genre.name} />
    </MenuItem>
  ))}
</Select> 
</FormControl>*/}