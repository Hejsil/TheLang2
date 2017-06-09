main :: () => {
    list: make_list(0)
    
    loop 0 .. 100 {
        list.add(item)
    }

    last: list.count - 1
    loop 0 .. list.count / 4 {
        list.remove(last - (item * 2))
    }

    list.delete()
}

make_list :: (default_values) => {
    return struct(
        count: 0 
        data: new [2]{ default_values }
    )
}

delete :: (list) => {
    delete list.data
}

[] :: (list, index) => {
    assert(0 <= index && index < list.count)
    return list.data[index]    
}

add :: (list, item) => {
    if (list.data.size == list.count + 1) {
        old: list.data
        list.data = new [old.size * 2]{ 0 }

        old.copy_to(list.data)
        delete old
    }

    list.data[list.count] = item
    list.count += 1
}

remove :: (list, index) => {
    assert(0 <= index && index < list.count)

    loop index .. list.count - 1 {
        list.data[item] = list.data[item + 1]
    }

    list.count -= 1
} 

copy_to :: (a1, a2) => {
    loop 0 .. a1.size {
        a2[item] = a1[item]
    }
}