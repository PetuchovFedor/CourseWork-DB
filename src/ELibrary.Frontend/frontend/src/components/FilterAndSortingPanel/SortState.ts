export enum SortState {
    None = 0,
    NameAsc = 1,
    NameDesc = 2,
    DateAsc = 3,
    DateDesc = 4,
    Rating = 5,
    Error = 6
}

export const sortStateMap: Map<SortState, string> = new Map([
    [SortState.None, 'Без сортировки'],
    [SortState.NameAsc, 'По возрастанию названия'],
    [SortState.NameDesc, 'По убыванию названия'],
    [SortState.DateAsc, 'По возрастанию даты'],
    [SortState.DateDesc, 'По убыванию даты'],
    [SortState.Rating, 'По рейтингу'],
])