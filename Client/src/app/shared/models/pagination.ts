export type pagination <T>={
    map(arg0: (p: any) => any): import("./product").Product[];
    pageIndex:number;
    pageSize:number;
    count:number;
    data:T[]
}