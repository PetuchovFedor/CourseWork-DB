import { useEffect, useState } from 'react'
import styles from './FilterAndSortingPanel.module.css'
import { SortState, sortStateMap } from './SortState'
import { useLocation, useNavigate } from 'react-router-dom'
import {GenresMenu} from '../GenresMenu/GenresMenu'
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import ListItemText from '@mui/material/ListItemText';
import Select from '@mui/material/Select';
import Checkbox from '@mui/material/Checkbox';

const sortStates: string[] = Array.from(sortStateMap.values())
const ITEM_HEIGHT = 25;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 150,
    },
  },
};

interface IFilterAndSortingPanel {
    genres: Array<number>,
    sortState: SortState
}

export function FilterAndSortingPanel(props: IFilterAndSortingPanel) {
    const [selectedSort, setSelect] = useState<string>('')
    const [genres, setGenres] = useState<Array<number>>(props.genres)
    const [sortState, setSortState] = useState<SortState>(props.sortState)
    const location = useLocation()
    const navigate = useNavigate()
    // console.log(location)
    // console.log(props)
    const handleChange = (event) => {       
        const {
          target: {value },
        } = event;
        if (value == 'Без сортировки') 
        {
            setSortState(SortState.None)
        }      
        else if (value == 'По возрастанию названия') {
            setSortState(SortState.NameAsc)
        }
        else if (value == 'По убыванию названия') {
            setSortState(SortState.NameDesc)
        }
        else if (value == 'По возрастанию даты') {
            setSortState(SortState.DateAsc)
        }
        else if (value == 'По убыванию даты') {
            setSortState(SortState.DateDesc)
        }
        else if (value == 'По рейтингу') {
            setSortState(SortState.Rating)
        }
        // setSelect(value)
        // setSortState(sortStateMap.get(selectedSort))
        // console.log(selectedSort)
    };
    // useEffect(() => {
    //     console.log(genres)
    // }, [genres])

    const apply = () => {
        const pathname: string = location.pathname
        console.log(genres)
        let navigateUrl: string = (pathname[pathname.length - 1] == '/' 
            ? pathname.substring(0, pathname.length - 1) : pathname) + '/?'
        for (let genre of genres) {
            navigateUrl += 'genres-ids=' + genre +  '&'
        } 
        const sort = sortState as number
        navigate(navigateUrl + 'sort-state=' + sortState)
        window.location.reload()
    }

    return (
        <div className={styles.panel}>
            <GenresMenu id={2} setGenres={setGenres} selectedGenres={props.genres}/>
            <FormControl sx={{ m: 1, width: 350 }}>
            <InputLabel id="demo-single-checkbox-label">Сортировка</InputLabel>
            <Select
            labelId="demo-single-checkbox-label"
            id="demo-single-checkbox"            
            // multiple 
            value={sortStateMap.get(sortState)}
            onChange={handleChange}
            input={<OutlinedInput label="Сортировка" />}
            renderValue={(selected) => selected}
            MenuProps={MenuProps}
            >
            {sortStates && sortStates.map((state) => (
            <MenuItem key={state} value={state}>
                <Checkbox checked={sortStateMap.get(sortState) == state} />
                <ListItemText primary={state} />
            </MenuItem>
          ))}
            </Select>
            </FormControl>
            <button onClick={apply} className={styles.apply_button}>Применить</button>
        </div>
    )
}