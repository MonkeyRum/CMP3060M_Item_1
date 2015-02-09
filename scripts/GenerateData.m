function GenerateData( filenames, outDir, w, h )
%GenerateData Generates feature data from input image.

numFiles = size(filenames);

funs = {
            @(block_struct) median(block_struct.data), ...
            @(block_struct) mean(block_struct.data), ...
            @(block_struct) std(block_struct.data), ...
            @(block_struct) mode(block_struct.data), ...
            @(block_struct) min(block_struct.data), ...
            @(block_struct) max(block_struct.data), ...
            @(block_struct) entropy(block_struct.data)
    };

numFuns = size(funs);
numFuns = numFuns(2);

for i=1:numFiles
    
    name = filenames{i, 1};
    I = imread(name);
    IR = I(:,:,1);
    IG = I(:,:,2);
    IB = I(:,:,3);
    
    for j=1:numFuns
        
        fun = funs{j};
        filename = filenames{i};
       
        blocks = blockproc(double(IR), [w, h], fun);
        csvwrite(strcat(outDir, filename, '.', 'RED.', func2str(fun), '.csv'), blocks);
        
        blocks = blockproc(double(IG), [w, h], fun);
        csvwrite(strcat(outDir, filename, '.', 'GREEN.', func2str(fun), '.csv'), blocks);
        
        blocks = blockproc(double(IB), [w, h], fun);
        csvwrite(strcat(outDir, filename, '.', 'BLUE.', func2str(fun), '.csv'), blocks);
        
    end
    
end

end

