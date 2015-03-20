function GenerateData( filenames, outDir, w, h )
%GenerateData Generates feature data from input image.

numFiles = size(filenames);
numFiles = numFiles(2);

funs = {
            @(block_struct) graycoprops_fun(block_struct.data, 'Contrast'), ...
            @(block_struct) graycoprops_fun(block_struct.data, 'Correlation'), ...
            @(block_struct) graycoprops_fun(block_struct.data, 'Energy'), ...
            @(block_struct) graycoprops_fun(block_struct.data, 'Homogeneity'), ...
            @(block_struct) mean2(block_struct.data), ...
            @(block_struct) median(median(block_struct.data)), ...
            @(block_struct) std(std(block_struct.data)), ...
            @(block_struct) mode(mode(block_struct.data)), ...
            @(block_struct) min(min(block_struct.data)), ...
            @(block_struct) max(max(block_struct.data))
    };

numFuns = size(funs);
numFuns = numFuns(2);

regex = ',';
replace = '.';

for i=1:numFiles
    
    name = filenames{1, i};
    I = imread(name);
    IR = I(:,:,1);
    IG = I(:,:,2);
    IB = I(:,:,3);
    IGray = rgb2gray(I);
    
    for j=1:numFuns
        
        fun = funs{j};
        filename = filenames{i};
        
        outFile = strcat(outDir, filename, '.', 'RED.', func2str(fun), '.csv');
        outFile = regexprep(outFile,regex,replace);
        blocks = blockproc(double(IR), [w, h], fun);
        csvwrite(outFile, blocks);
        
        outFile = strcat(outDir, filename, '.', 'GREEN.', func2str(fun), '.csv');
        outFile = regexprep(outFile,regex,replace);
        blocks = blockproc(double(IG), [w, h], fun);
        csvwrite(outFile, blocks);
        
        outFile = strcat(outDir, filename, '.', 'BLUE.', func2str(fun), '.csv');
        outFile = regexprep(outFile,regex,replace);
        blocks = blockproc(double(IB), [w, h], fun);
        csvwrite(outFile, blocks);
        
        outFile = strcat(outDir, filename, '.', 'GRAY.', func2str(fun), '.csv');
        outFile = regexprep(outFile,regex,replace);
        blocks = blockproc(double(IGray), [w, h], fun);
        csvwrite(outFile, blocks);
        
    end
    
end

end

function [out_val] = graycoprops_fun (block, param)

    out_val = graycoprops(block, param);
    out_val = getfield(out_val, param);
    
end