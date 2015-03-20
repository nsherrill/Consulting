
select sum(iserverguestcount)
	from journal
	where istatus_pullback=0
		and dttransdate > '{DESIREDDATESTART}'
		and dttransdate < '{DESIREDDATEEND}'
		and bforcedclose is null